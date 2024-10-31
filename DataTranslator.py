from rich.progress import Progress, TextColumn
from rich.console import Console

from uuid import uuid4 as uuid
from datetime import datetime
import os
import json

from src.horse import Horse

console = Console()
horses = []

PATH = input("Enter the path to the data file: ")


with Progress(
        *Progress.get_default_columns(),
        TextColumn("[bold blue]{task.fields[horse_name]}", justify="right")
    ) as progress:
    with open(PATH, "r") as file:
        data = file.readlines()
    translation_task = progress.add_task(
        "[green]Translating Data...",
        total=len(data),
        horse_name="Starting..."
    )

    for line in data:
        line = line.strip().split(",")
        name = line[0]
        # Parse the date using the correct format
        last_shod = datetime.strptime(line[1], "%m/%d/%Y %I:%M:%S %p").date()
        rotation_interval = line[2]
        is_trim = line[3]
        horses.append(
            Horse(
                name,
                uuid(),
                bool(is_trim),
                int(rotation_interval),
                last_shod)
        )
        with open("tools/horses.json", "w") as file:
            horse_data = []
            for horse in horses:
                horse_data.append({
                    "name": horse.name,
                    "id": str(horse.id),
                    "is_trim": horse.is_trim,
                    "rotation_interval": horse.rotation_interval,
                    "last_shoe_date": horse.last_shoe_date.isoformat()
                })
            file.write(json.dumps(horse_data))
        progress.update(translation_task, advance=1, horse_name=name)