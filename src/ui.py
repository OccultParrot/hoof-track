import keyboard
from rich.console import Console
import datetime
# from uuid import uuid4
# import time
import os

from src.data import Data
# from src.horse import Horse

console = Console()

database: Data


def catch_key_main(event: keyboard.KeyboardEvent) -> bool:
    key = event.name
    # Put key logic here!
    if key == "q" or key == "esc":
        return False
    elif key == "f1":
        console.log("F1!!!")
        return True

    return True  # Return False to exit the loop


def draw_starting_screen():
    os.system('cls' if os.name == 'nt' else 'clear')
    console.print("FARRIER BUSINESS MANAGEMENT SYSTEMS", justify="center")
    console.print()
    console.rule()
    console.print()
    console.print(f'{datetime.date.today().day}/{datetime.date.today().month}/{datetime.date.today().year}',
                  justify="center")
    console.print("Horses active in rotation: [TODO]", justify="center")
    console.print("Horses overdue: [TODO]", justify="center")


def run(data: Data):
    global database
    database = data

    is_running = True
    draw_starting_screen()
    while is_running:
        event = keyboard.read_event(suppress=True)
        if event.event_type == keyboard.KEY_DOWN:
            is_running = catch_key_main(event)
            draw_starting_screen()
