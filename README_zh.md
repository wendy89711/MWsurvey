# 病房使用滿意度調查系統

- [English Version](README.md)
- 以 **ASP.NET MVC 5** 為架構的 **病房使用滿意度調查系統**。 

![Project Demo](demo_image/UI_US5NET.png)

## 系統需求
- **IDE**：Visual Studio 2019 或 2022（含 .NET 開發工作負載）
- **資料庫**（若有）：SQL Server / SQL Server Express / LocalDB 皆可



## 資料夾結構
```bash
MWsurvey/                  
├─ Controllers
│  ├─ HomeController.cs     # 定義網站首頁控制器，只提供 Index() 來回傳首頁檢視
│  └─ SurveyController.cs   # 處理整個問卷流程（依分類載入問卷之題目與選項、檢核使用者、儲存作答，最後顯示成功頁）
├─ Models                   # 自動產生檔
├─ Views
│  ├─ Home                  # 首頁
│  ├─ Shared                # 共享頁面，如：錯誤頁面
│  └─ Survey                # 各個使用者分類的問卷
└─ ...                      
```
