The implementation of the QRCodeReader is pretty much stupid - if you add more than 1 control on the form/view,
you will not liek the result:)

The control is a simple input type file control with a behind logic, which transfers the received image and tries
to decode it with the help of jquery libraries, taken form here https://github.com/zxing/zxing and from here
http://www.webqr.com/index.html.

The control hase 1 OnChange event, which can be used to transfer the decoded information to other datafields etc.

Certainly, because the control does read the QRCode in real time, the photo you make should be of good quality and
the QRCode must take the most of its space.

Tested on PC with files from the internet and also on iPad (Safari), making photos of various QRCodes. 

In general, this is it...
