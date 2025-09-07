@echo off
chcp 65001>nul

REM 文件复制助手编译脚本
REM Build script for FileCopyHelper project

echo ========================================
echo FileCopyHelper 编译脚本
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

REM 编译Debug版本
echo.
echo 正在编译 Debug 版本...
dotnet build -c Debug
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
dotnet build -c Release
if %errorlevel% neq 0 (
    echo Release 版本编译失败!
    pause
    exit /b %errorlevel%
) else (
    echo Release 版本编译成功!
)

REM 发布应用程序
echo.
echo 正在发布应用程序...
dotnet publish -c Release --self-contained false
if %errorlevel% neq 0 (
    echo 应用程序发布失败!
    pause
    exit /b %errorlevel%
) else (
    echo 应用程序发布成功!
)

REM 复制必要的文件到输出目录
echo.
echo 正在复制必要的文件...

REM 创建发布目录
set PUBLISH_DIR=Publish
if exist "%PUBLISH_DIR%" rmdir /s /q "%PUBLISH_DIR%" >nul 2>&1
mkdir "%PUBLISH_DIR%" >nul 2>&1

REM 复制Release版本的可执行文件和依赖项
set RELEASE_DIR=bin\Release\net472
set PUBLISH_DIR_NET=bin\Release\net472\publish
if exist "%PUBLISH_DIR_NET%" (
    xcopy "%PUBLISH_DIR_NET%\*" "%PUBLISH_DIR%\" /E /I /Y >nul
    echo 已复制发布版本文件到发布目录
) else if exist "%RELEASE_DIR%" (
    xcopy "%RELEASE_DIR%\*" "%PUBLISH_DIR%\" /E /I /Y >nul
    echo 已复制 Release 版本文件到发布目录
) else (
    echo 警告: 未找到发布输出目录
)

REM 显示构建结果
echo.
echo ========================================
echo 编译完成!
echo ========================================
echo Debug 版本位置: bin\Debug\net472
echo Release 版本位置: bin\Release\net472
echo 发布版本位置: bin\Release\net472\publish
echo 最终发布目录: Publish
echo.

REM 询问是否运行程序
set /p RUN_PROGRAM=是否要运行程序? (y/n): 
if /i "%RUN_PROGRAM%"=="y" (
    echo 正在启动程序...
    if exist "%PUBLISH_DIR%\FileCopyHelper.exe" (
        start "" "%PUBLISH_DIR%\FileCopyHelper.exe"
    ) else if exist "bin\Release\net472\FileCopyHelper.exe" (
        start "" "bin\Release\net472\FileCopyHelper.exe"
    ) else (
        echo 未找到可执行文件
    )
)

pause