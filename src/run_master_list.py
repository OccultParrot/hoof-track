import os
import sys
import subprocess
import time

def run_master_list():
    try:
        # Check if we're running from a PyInstaller bundle
        if getattr(sys, 'frozen', False):
            # Running from PyInstaller exe
            base_path = sys._MEIPASS
        else:
            # Running from normal Python
            base_path = os.path.join(os.path.dirname(__file__))

        exe_path = os.path.join(base_path, "MasterList.exe")
        exe_path = os.path.abspath(exe_path)

        print(f"Attempting to launch from: {exe_path}")  # For debugging

        subprocess.Popen([exe_path], creationflags=subprocess.CREATE_NEW_CONSOLE)
        return True
    except Exception as e:
        print(f"Error launching program: {e}")
        print(f"Base path was: {base_path}")
        print(f"Attempted exe path was: {exe_path}")

        time.sleep(1)
        return False
