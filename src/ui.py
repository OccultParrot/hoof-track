import keyboard
from rich.console import Console
import datetime
import os
from typing import Callable

from src.data import Data

console = Console()


def add_horse():
    console.log("Caught add horse")
    input("Press Enter to continue...")


def remove_horse():
    console.log("Caught remove horse")
    input("Press Enter to continue...")


def barn_menu():
    console.log("Caught barn menu")
    input("Press Enter to continue...")


def master_list():
    console.log("Caught master list")
    input("Press Enter to continue...")


def catch_key_main(event: keyboard.KeyboardEvent, database: Data) -> bool:
    key = event.name

    if key in ["q", "esc"]:
        return False

    key_actions: dict[str, Callable[[], None]] = {
        database.get_key("add_horse"): add_horse,
        database.get_key("remove_horse"): remove_horse,
        database.get_key("barn_menu"): barn_menu,
        database.get_key("master_list"): master_list,
    }

    if key in key_actions:
        key_actions[key]()

    return True


def draw_starting_screen(database: Data):
    os.system('cls' if os.name == 'nt' else 'clear')
    console.print("FARRIER BUSINESS MANAGEMENT SYSTEMS", justify="center")
    console.print()
    console.rule()
    console.print()
    # TODO: Add day of the week
    console.print(f'DAY OF THE WEEK HERE, {datetime.date.today().strftime("%d/%m/%Y")}', justify="center")
    console.print(f"Horses active in rotation: {len(database.horses)}", justify="center")
    console.print(f"Horses overdue: {sum(1 for horse in database.horses if horse.get_weeks_overdue() > 0)}",
                  justify="center")


def run(database: Data):
    is_running = True
    draw_starting_screen(database)

    while is_running:
        event = keyboard.read_event(suppress=True)
        if event.event_type == keyboard.KEY_DOWN:
            is_running = catch_key_main(event, database)
            draw_starting_screen(database)