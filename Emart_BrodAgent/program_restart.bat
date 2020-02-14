@echo off

TaskKill /F /IM Emart_BrodAgent.exe

cd /d %~dp0

start Emart_BrodAgent.exe


@Echo off

TaskKill /F /IM cmd.exe

exit