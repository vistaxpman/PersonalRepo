declare @Person table
(
    Id int
   ,Name varchar(10)
);
insert into
    @Person
values
    (1, 'Dong Jia')
   ,(2, 'Yan Zhang');

declare @Cash table
(
    Id int
   ,Date datetime
   ,Cash float
);
insert into
    @Cash
values
    (1, '2011/11/4', 500)
   ,(1, '2011/11/5', 600);

select
    P.Id
   ,P.Name
   ,C.Date
   ,C.Cash
from
    @Person P
    left join @Cash C
    on  C.Id = P.Id
        and
        C.Date =
        (
            select
                MAX(Date)
            from
                @Cash
            where
                C.Id = P.Id
        )