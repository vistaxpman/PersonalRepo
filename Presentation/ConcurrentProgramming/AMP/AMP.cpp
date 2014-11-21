// C++ Accelerated Massive Parallelism
// Present on C++ AMP 
// http://blogs.msdn.com/b/nativeconcurrency/archive/2012/08/30/learn-c-amp.aspx
// Learn C++ AMP
// http://blogs.msdn.com/b/nativeconcurrency/archive/2012/08/30/learn-c-amp.aspx

#include <iostream>
#include <vector>

#include <assert.h>
#include <amp.h>
#include <tchar.h>
#include <time.h>

using namespace concurrency;

#define DATA_TYPE float

//----------------------------------------------------------------------------
// Generate random data
//----------------------------------------------------------------------------
template<typename _type>
void initialize_array(std::vector<_type> &v_data, unsigned size)
{
    for(unsigned i = 0; i < size; ++i)
    {
        v_data[i] = (_type)((_type)rand() * 100 / (_type)(RAND_MAX + 1));
    }
}

//----------------------------------------------------------------------------
// Implement matrix multiplication on CPU - N cube algorithm
//----------------------------------------------------------------------------
template<typename _type>
void mxm_single_cpu(int M, int N, int W, const std::vector<_type>& va, const std::vector<_type>& vb, std::vector<_type>& vresult)
{
    if ((va.size() != M*N) || (vb.size() != N*W) || (vresult.size() != M*W))
        throw "Expected matrix dimension result(M*W) = a(M*N) * b(N*W)";

    for(int k = 0; k < M; ++k)
    {
        for(int j = 0; j < W; ++j)
        {
            _type result = 0;

            for(int i = 0; i < N; ++i)
            {
                int idx_a = k * N + i;
                int idx_b = i * W + j;

                result += va[idx_a] * vb[idx_b];
            }

            vresult[k * W + j] = result;
        }
    }
}

//----------------------------------------------------------------------------
// Implement simple matrix multiplication on GPU using C++ AMP
//----------------------------------------------------------------------------
template<typename _type>
void mxm_amp_simple(int M, int N, int W, const std::vector<_type>& va, const std::vector<_type>& vb, std::vector<_type>& vresult)
{
    if ((va.size() != M*N) || (vb.size() != N*W) || (vresult.size() != M*W))
        throw "Expected matrix dimension result(M*W) = a(MxN) * b(N*W)";

    extent<2> e_a(M, N), e_b(N, W), e_c(M, W);

    // Copy in
    array_view<const _type, 2> av_a(e_a, va); 
    array_view<const _type, 2> av_b(e_b, vb); 
    array_view<_type, 2> av_c(e_c, vresult);
    av_c.discard_data();

    // Compute - outer 2 for loops of CPU is replaced by a parallel_for_each
    parallel_for_each(av_c.extent,
        [=](index<2> idx) restrict(amp)
        {
            _type result = 0;

            for(int i = 0; i < av_a.extent[1]; ++i)
            {
                index<2> idx_a(idx[0], i);
                index<2> idx_b(i, idx[1]);

                result += av_a[idx_a] * av_b[idx_b];
            }

            av_c[idx] = result;
        });

    // explicitly about copying out data
    av_c.synchronize();
}

//----------------------------------------------------------------------------
// Implement tiled version of matrix multiplication
//----------------------------------------------------------------------------
template<typename _type, int tile_size>
void mxm_amp_tiled(int M, int N, int W, const std::vector<_type>& va, const std::vector<_type>& vb, std::vector<_type>& vresult)
{
    if ((va.size() != M*N) || (vb.size() != N*W) || (vresult.size() != M*W))
        throw "Expected matrix dimension result(M*W) = a(MxN) * b(N*W)";

    extent<2> e_a(M, N), e_b(N, W), e_c(M, W);

    assert((M%tile_size) == 0);
    assert((W%tile_size) == 0);
    assert((N%tile_size) == 0);

    // Copy in
    array_view<const _type, 2> av_a(e_a, va); 
    array_view<const _type, 2> av_b(e_b, vb); 
    array_view<_type, 2> av_c(e_c, vresult);

    extent<2> compute_domain(e_c);

    parallel_for_each(compute_domain.tile<tile_size, tile_size>(),
        [=] (tiled_index<tile_size, tile_size> tidx) restrict(amp) 
        {
            _type temp_c = 0;

            index<2> localIdx = tidx.local;
            index<2> globalIdx = tidx.global;
  
            for (int i = 0; i < N; i += tile_size)
            {
                tile_static _type localB[tile_size][tile_size];
                tile_static _type localA[tile_size][tile_size];

                localA[localIdx[0]][localIdx[1]] = av_a(globalIdx[0], i + localIdx[1]);
                localB[localIdx[0]][localIdx[1]] = av_b(i + localIdx[0], globalIdx[1]);
        
                tidx.barrier.wait();
        
                for (unsigned k = 0; k < tile_size; k++)
                {
                    temp_c += localA[localIdx[0]][k] * localB[k][localIdx[1]];
                }
       
                tidx.barrier.wait();
            }

            av_c[tidx] = temp_c;
        });

    // copying out data is implicit - when array_view goes out of scope data is synchronized
}

template<typename _type>
bool verify(std::vector<_type>& v_res, std::vector<_type>& v_ref, int len)
{
    bool passed = true;

    for (int i = 0; i < len; ++i)
    {
        if (v_res[i] != v_ref[i])
        {
             printf("v_res[%d] = %f, v_ref[%d] = %f\n", i, v_res[i], i, v_ref[i]);
             passed = false;
             break;
        }
    }

    return passed;
}

template<>
bool verify(std::vector<float>& v_res, std::vector<float>& v_ref, int len)
{
    bool passed = true;

    for (int i = 0; i < len; ++i)
    {
        if (fabs(v_res[i] - v_ref[i]) > 0.01)
        {
             printf("v_res[%d] = %f, v_ref[%d] = %f\n", i, v_res[i], i, v_ref[i]);
             passed = false;
             break;
        }
    }

    return passed;
}

template<>
bool verify(std::vector<double>& v_res, std::vector<double>& v_ref, int len)
{
    bool passed = true;

    for (int i = 0; i < len; ++i)
    {
        if (fabs(v_res[i] - v_ref[i]) > 0.01)
        {
             printf("v_res[%d] = %f, v_ref[%d] = %f\n", i, v_res[i], i, v_ref[i]);
             passed = false;
             break;
        }
    }

    return passed;
}

int SPINCOUNT = 25;
//----------------------------------------------------------------------------
// Spins for a fixed number of loops
//----------------------------------------------------------------------------
#pragma optimize("", off)
void delay()
{
    for (int i = 0; i < SPINCOUNT; ++i)
    {
    }
};
#pragma optimize("", on)

//----------------------------------------------------------------------------
// Times execution of a functor in ms
//----------------------------------------------------------------------------
template <class Functor>
__int64 time_call(Functor& fn)
{
    __int64 begin, end;
    begin = GetTickCount();
    fn();
    end = GetTickCount();
    return end - begin;
};

int _tmain(int argc, _TCHAR* argv[])
{
    accelerator default_device;
    std::wcout << L"Using device : " << default_device.get_description() << std::endl;
    if (default_device == accelerator(accelerator::direct3d_ref))
        std::cout << "WARNING!! Running on very slow emulator! Only use this accelerator for debugging." << std::endl;

    srand((unsigned int)time(NULL));

    const int M = 128;
    const int N = 128;
    const int W = 128;
    
    std::vector<DATA_TYPE> v_a(M * N);
    std::vector<DATA_TYPE> v_b(N * W);
    std::vector<DATA_TYPE> v_c_simple(M * W);
    std::vector<DATA_TYPE> v_c_tiled(M * W);
    std::vector<DATA_TYPE> v_ref(M * W);

    initialize_array(v_a, M * N);
    initialize_array(v_b, N * W);

    assert((M!=0) && (W!=0) && (N!=0));

    printf("Matrix dimension C(%d x %d) = A(%d x %d) * B(%d x %d)\n", M, W, M, N, N, W);
    printf("CPU(single core) exec ");
    double time = (double) time_call(
        [&] ()
        { mxm_single_cpu(M, N, W, v_a, v_b, v_ref); } );
    printf("completed in %.2f ms\n", time);

    printf("AMP Simple ");
    time = (double) time_call(
        [&] ()
        { mxm_amp_simple(M, N, W, v_a, v_b, v_c_simple); });
    printf("completed in %.2f ms.\n", time);
    printf("\t%s\n\n", verify(v_c_simple, v_ref, M * W) ? "Data matches" : "Data mismatch");

    printf("AMP Tiled ");
    time = (double) time_call(
        [&] ()
        { mxm_amp_tiled<DATA_TYPE, 16>(M, N, W, v_a, v_b, v_c_tiled); } );
    printf("completed in %.2f ms.\n", time);
    printf("\t%s\n\n", verify(v_c_tiled, v_ref, M * W) ? "Data matches" : "Data mismatch");

    ::getchar();
    return 0;
}
