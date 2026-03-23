import os
import json
import subprocess
from datetime import datetime

# === 設定區 ===
DOWNLOAD_DIR = "Downloads"
FILENAME = f"test_data_{datetime.now().strftime('%Y%m%d_%H%M%S')}.json"

def download_file(save_dir, filename):
    """模擬下載：產生本地測試 JSON 檔"""
    os.makedirs(save_dir, exist_ok=True)
    save_path = os.path.join(save_dir, filename)
    data = {
        "status": "ok",
        "message": "模擬下載成功（本地產生）",
        "timestamp": datetime.now().isoformat()
    }
    with open(save_path, "w", encoding="utf-8") as f:
        json.dump(data, f, ensure_ascii=False, indent=2)
    print(f"[完成] 檔案已產生: {save_path}")
    return save_path

def git_commit(filepath, message):
    subprocess.run(["git", "add", filepath], check=True)
    result = subprocess.run(["git", "status", "--short"], capture_output=True, text=True)
    if result.stdout.strip():
        subprocess.run(["git", "commit", "-m", message], check=True)
        print(f"[Commit] {message}")
    else:
        print("[跳過] 沒有需要 commit 的變更")

if __name__ == "__main__":
    print("=== 自動下載 + 自動 Commit 測試腳本 ===")
    saved_path = download_file(DOWNLOAD_DIR, FILENAME)
    commit_msg = f"auto: 下載測試檔案 {FILENAME} [{datetime.now().strftime('%Y-%m-%d %H:%M:%S')}]"
    git_commit(saved_path, commit_msg)
    print("=== 完成 ===")
