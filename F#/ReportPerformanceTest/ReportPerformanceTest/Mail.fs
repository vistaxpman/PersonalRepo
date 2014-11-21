module Mail

open Microsoft.Office.Interop.Outlook

let private application = new ApplicationClass()
let private mailItem = application.CreateItem(OlItemType.olMailItem) :?> MailItem

let sendResult(tos, subject, body, attachments) =
    mailItem.To <- tos |> List.fold (fun acc x ->
        acc + x + "; "
    ) ""
    mailItem.Subject <- subject
    mailItem.Body <- body

    attachments |> List.iter (fun x ->
        mailItem.Attachments.Add(x, OlAttachmentType.olByValue) |> ignore
    )

    mailItem.Send()