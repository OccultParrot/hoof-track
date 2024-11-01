import keyboard
from pygments.lexer import default
from rich.console import Console
import datetime
import time
import os
from typing import Callable

from prompt_toolkit import PromptSession
from prompt_toolkit.shortcuts import confirm
from prompt_toolkit.completion import Completer, Completion

from src.horse import Horse, new_horse
from src.validators.NumberValidator import NumberValidator
from src.validators.DateValidator import DateValidator
from src.run_master_list import run_master_list
from src.data import Data

data = Data()

console = Console()


# Use this anytime the user needs to select a horse!
class HorseCompleter(Completer):
    def get_completions(self, document, complete_event):
        for horse in data.horses:
            yield Completion(horse.name, start_position=-document.cursor_position)


def add_horse():
    session = PromptSession()
    # Name
    # Trim
    # Rotation Interval
    # Set Shod date to today or custom
    console.print("What is the name of the horse?")
    horse_name = session.prompt('> ')

    console.print("Is the horse a trim?")
    horse_is_trim = confirm("", suffix="(y/n) > ")

    console.print("How often do you shod the horse? (In weeks)")
    horse_rotation_interval = session.prompt("> ", default="6",
                                             validator=NumberValidator(), validate_while_typing=True)
    console.print("When was the horse last shod?")
    console.print("(Enter no to input a custom date)")
    shod_date_choice = confirm("", suffix="(y/n) > ")
    if shod_date_choice:
        horse_shod_date = datetime.date.today()
        print(horse_shod_date)
    else:
        horse_shod_date = session.prompt("Enter a custom date", validator=DateValidator(), validate_while_typing=True)

    horse = new_horse(horse_name, horse_is_trim, horse_rotation_interval, horse_shod_date)

    data.add_horse(horse)
    console.print("[yellow]Added Horse")
    time.sleep(0.5)


def remove_horse():
    session = PromptSession()
    horse_name = session.prompt('What is the horses name?', completer=HorseCompleter(), complete_while_typing=True,
                                bottom_toolbar="Press right arrow to autocomplete")
    data.remove_horse(horse_name)
    console.print("[yellow]Removed Horse")
    time.sleep(0.5)


def barn_menu():
    console.log("Caught barn menu")
    input("Press Enter to continue...")



def catch_key_main(event: keyboard.KeyboardEvent, database: Data) -> bool:
    key = event.name

    if key in ["q", "esc"]:
        return False

    key_actions: dict[str, Callable[[], None]] = {
        database.get_key("add_horse"): add_horse,
        database.get_key("remove_horse"): remove_horse,
        database.get_key("barn_menu"): barn_menu,
        database.get_key("master_list"): run_master_list,
    }
    if key in key_actions:
        key_actions[key]()

    return True


def draw_starting_screen(database: Data):
    days_of_the_week = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday']
    os.system('cls' if os.name == 'nt' else 'clear')
    console.print("F1 to add a horse, F2 to remove a horse, F3 to view barns (NOT WORKING YET), F4 to view the master list", justify="center")
    console.print("\n\n")
    console.print("FARRIER BUSINESS MANAGEMENT SYSTEMS", justify="center")
    console.print()
    console.rule(style="white")
    console.print()
    console.print(f'{days_of_the_week[datetime.date.today().weekday()]}, {datetime.date.today().strftime("%d/%m/%Y")}', justify="center")
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
            data.save_horses("db/horses.json")
            draw_starting_screen(database)
