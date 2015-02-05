
REM
echo this batch file registers the custom control assembly with K2 smartforms
pause
iisreset
xcopy "QRCodeReader.dll" "C:\Program Files (x86)\K2 blackpearl\K2 SmartForms Designer\bin\" /y /r
xcopy "QRCodeReader.dll" "C:\Program Files (x86)\K2 blackpearl\K2 SmartForms Runtime\bin\" /y /r
@SET CMD="C:\Program Files (x86)\K2 blackpearl\Bin\controlutil.exe" register -assembly:"C:\Program Files (x86)\K2 blackpearl\K2 SmartForms Designer\bin\QRCodeReader.dll"
%CMD%