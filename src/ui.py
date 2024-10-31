import keyboard
from prompt_toolkit.auto_suggest import AutoSuggest, AutoSuggestFromHistory
from prompt_toolkit.history import InMemoryHistory
from rich.console import Console
import datetime
import os
from typing import Callable

from prompt_toolkit import PromptSession
from prompt_toolkit.shortcuts import confirm

from src.horse import Horse, new_horse
from src.validators.NumberValidator import NumberValidator
from src.validators.DateValidator import DateValidator

from src.data import Data

data = Data()

console = Console()

def add_horse():
    session = PromptSession()
    # Name
    # Trim
    # Rotation Interval
    # Set Shod date to today or custom
    horse_name = session.prompt('What is the horses name?')
    horse_is_trim = confirm("Is the horse a trim?")
    horse_rotation_interval = session.prompt("How often do you shod the horse? (In weeks)",default="6", validator=NumberValidator(), validate_while_typing=True)
    shod_date_choice = confirm("Shod today? (Enter no to input a custom date)")
    if shod_date_choice:
        horse_shod_date = datetime.date.today()
        print(horse_shod_date)
    else:
        horse_shod_date = session.prompt("Enter a custom date", validator=DateValidator(), validate_while_typing=True)

    horse = new_horse(horse_name, horse_is_trim, horse_rotation_interval, horse_shod_date)

    data.add_horse(horse)
    console.print("[yellow]Added Horse")


def remove_horse():
    session = PromptSession(history=InMemoryHistory(data.get_horses_as_strings()))
    horse_name = session.prompt('What is the horses name?', auto_suggest=AutoSuggestFromHistory(), bottom_toolbar="Press right arrow to autocomplete")
    data.remove_horse(horse_name)


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
    global data
    data = database
    is_running = True
    draw_starting_screen(database)

    while is_running:
        event = keyboard.read_event(suppress=True)
        if event.event_type == keyboard.KEY_DOWN:
            is_running = catch_key_main(event, database)
            draw_starting_screen(database)