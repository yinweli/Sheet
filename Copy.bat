echo off
set pathSource=SheetBuilder\bin\%1\netcoreapp3.1\*.*
set pathTarget=Output\SheetBuilder\
echo on

echo %pathSource%
echo %pathTarget%

rmdir /q/s "%pathTarget%"
xcopy "%pathSource%" "%pathTarget%" /e/y