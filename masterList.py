from src.horse import Horse
import json
from uuid import UUID
import os
from rich.console import Console
from rich.table import Table

console = Console()


def clear():
    os.system('cls' if os.name == 'nt' else 'clear')


def refresh_info(horses):
    clear()
    table = Table(show_header=True, header_style="bold magenta", title="Horses")
    table.add_column("Name", justify="right")
    table.add_column("Rotation Interval", justify="right", style="Yellow")
    table.add_column("Weeks Overdue", justify="right", style="Red")

    for horse in horses:
        table.add_row(horse.name, horse.rotation_interval, horse.get_weeks_overdue())
        console.print(table)




def run():
    data = []
    # Catching if the data folder exists, cause if it doesn't then there is no need to run this
    if not os.path.exists("db/"):
        print("Error! Could not find data folder! Make sure to run the main program first!")
        input("Press Enter to close...")
        return False


    horses = []
    with open("db/horses.json", "r") as file:
        while True:
            horse_data: [] = json.loads(file.read())
            for horse in horse_data:
                console.log(horse)
                horses.append(Horse(horse["name"],
                                    # Casting to the UUID class
                                    UUID(horse["id"]),
                                    horse["is_trim"],
                                    horse["rotation_interval"],
                                    horse["last_shoe_date"]))
            if data != horses:
                data = horses
                refresh_info(data)
            else:
                continue




if __name__ == '__main__':
    run()
