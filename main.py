from src.ui import run
from src.data import Data

data = Data()
try:
    data.load("db/")
except Exception as e:
    print("Error:", e)
    input("Press Enter to close application...")
    exit(1)

run(data)
