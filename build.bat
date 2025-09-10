@echo off
chcp 65001>nul

REM 文件助手编译脚本
REM Build script for FileHelper project

echo ========================================
echo FileHelper 编译脚本
echo ========================================
echo.

REM 设置工作目录为脚本所在目录
cd /d "%~dp0"

REM 检查是否安装了.NET SDK
echo 检查 .NET SDK 环境...
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo 错误: 未检测到 .NET SDK，请先安装 .NET SDK 8.0.0 或更高版本
    pause
    exit /b 1
) else (
    for /f "tokens=*" %%i in ('dotnet --version') do set DOTNET_VERSION=%%i
    echo 检测到 .NET SDK 版本: %DOTNET_VERSION%
)

REM 清理之前的构建
echo.
echo 正在清理之前的构建...
if exist "bin" rmdir /s /q "bin" >nul 2>&1
if exist "obj" rmdir /s /q "obj" >nul 2>&1

REM 创建构建目录
mkdir "bin" >nul 2>&1
mkdir "obj" >nul 2>&1

REM 还原NuGet包
echo.
echo 正在还原 NuGet 包...
dotnet restore FileHelper.csproj
if %errorlevel% neq 0 (
    echo NuGet 包还原失败!
    pause
    exit /b %errorlevel%
) else (
    echo NuGet 包还原成功!
)

REM 编译Debug版本
echo.
echo 正在编译 Debug 版本...
dotnet build FileHelper.csproj -c Debug
if %errorlevel% neq 0 (
    echo Debug 版本编译失败!
    pause
    exit /b %errorlevel%
) else (
    echo Debug 版本编译成功!
)

REM 编译Release版本
echo.
echo 正在编译 Release 版本...
dotnet build FileHelper.csproj -c Release
if %errorlevel% neq 0 (
    echo Release 版本编译失败!
    pause
    exit /b %errorlevel%
) else (
    echo Release 版本编译成功!
)


REM 显示构建结果
echo.
echo ========================================
echo 编译完成!
echo ========================================
echo Debug 版本位置: bin\Debug\net472
echo Release 版本位置: bin\Release\net472
echo.
