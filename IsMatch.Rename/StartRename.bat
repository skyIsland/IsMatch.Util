@echo off
if not exist IsMatch.Rename.dll (echo 请在发布后的目录下运行此文件，按任意键退出！&pause>nul&exit)
dotnet IsMatch.Rename.dll