open System.Drawing
open System.Windows.Forms;

type TextContext = {
    Text : string
    Font : Font
}

type ScreenElement =
    | TextElement of TextContext * RectangleF
    | ImageElement of string * RectangleF

let fntText = new Font("Calibri", 12.0f)
let fntHead = new Font("Calibri", 15.0f)

let parts = [
    TextElement(
        {
            Text = "Functional Programming for the Real World"; Font = fntHead
        },
        new RectangleF(5.0f, 0.0f, 410.0f, 30.0f)
    )
    ImageElement(
        "Cover.jpg",
        new RectangleF(120.0f, 30.0f, 150.0f, 200.0f)
    )
    TextElement(
        {
            Text = "In this book, we'll introduce you to the essential "+
                    "concepts of functional programming, but thanks to the .NET "+
                    "Framework, we won't be limited to theoretical examples. We'll "+
                    "use many of the rich .NET libraries to show how functional "+
                    "programming can be used in the real-world.";
            Font = fntText
        }, 
        new RectangleF(10.0f, 230.0f, 400.0f, 400.0f)
    )
]

let drawElements elements (g:Graphics) =
    for e in elements do
        match e with
            | TextElement (context, boundingBox) ->
                g.DrawString(context.Text, context.Font, Brushes.Black, boundingBox)
            | ImageElement (imagePath, boundingBox) ->
                let bmp = new Bitmap(imagePath)
                g.DrawImage(bmp, boundingBox)

let drawImage (width:int, height:int) space coreDrawingFunc =
    let bmp = new Bitmap(width, height)
    use g = Graphics.FromImage(bmp)
    g.Clear(Color.White)
    g.TranslateTransform(space, space)
    coreDrawingFunc(g)
    bmp

let docImage = drawImage(450, 400) 20.0f (drawElements parts)

let main = new Form(Name = "Document", BackgroundImage = docImage, Width = docImage.Width, Height = docImage.Height);

main.ShowDialog() |> ignore

