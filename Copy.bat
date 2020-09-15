echo off
set pathSource=StaticDataBuilder\bin\%1\netcoreapp3.1\*.*
set pathTarget=_Output\StaticDataBuilder\
echo on

echo %pathSource%
echo %pathTarget%

rmdir /q/s "%pathTarget%"
xcopy "%pathSource%" "%pathTarget%" /e/y