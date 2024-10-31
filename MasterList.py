from src.horse import Horse
from rich.console import Console
from rich.table import Table
import os
import json
import time
from uuid import UUID
from datetime import datetime

console = Console()


def clear():
    os.system('cls' if os.name == 'nt' else 'clear')


def refresh_info(horses):
    clear()
    table = Table(show_header=True, header_style="bold magenta", title="Horses")
    table.add_column("Name", justify="left")
    table.add_column("Rotation Interval", justify="right", style="Yellow")
    table.add_column("Weeks Overdue", justify="right", style="Red")

    for horse in horses:
        table.add_row((horse.name + (" (T)" if horse.is_trim else "")), str(horse.rotation_interval), str(horse.get_weeks_overdue()))

    console.print(table, justify="center", )



def run():
    if not os.path.exists("db/"):
        print("Error! Could not find data folder! Make sure to run the main program first!")
        input("Press Enter to close...")
        return False

    last_modified_time = 0
    data = []

    while True:
        try:
            # Get the file's last modification time
            current_modified_time = os.path.getmtime("db/horses.json")

            # Only read and process if the file has been modified
            if current_modified_time > last_modified_time:
                with open("db/horses.json", "r") as file:
                    horse_data = json.loads(file.read())
                    horses = [
                        Horse(
                            horse["name"],
                            UUID(horse["id"]),
                            bool(horse["is_trim"]),
                            int(horse["rotation_interval"]),
                            datetime.fromisoformat(horse["last_shoe_date"]).date()
                        )
                        for horse in horse_data
                    ]

                    # Update the display
                    refresh_info(horses)
                    data = horses
                    last_modified_time = current_modified_time

            # Add a small delay to prevent excessive CPU usage
            time.sleep(1)

        except Exception as e:
            print(f"Error reading file: {e}")
            time.sleep(5)  # Longer delay if there's an error
            continue




if __name__ == '__main__':
    run()
