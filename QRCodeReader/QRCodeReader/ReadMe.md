The implementation of the QRCodeReader is pretty much stupid - if you add more than 1 control on the form/view,
you will not liek the result:)

The control is a simple input type file control with a behind logic, which transfers the received image and tries
to decode it with the help of jquery libraries, taken form here https://github.com/zxing/zxing and from here
http://www.webqr.com/index.html.

The control hase 1 OnChange event, which can be used to transfer the decoded information to other datafields etc.

In general, this is it...
