from src.horse import Horse
import json
import time
import os
from uuid import UUID


def _generate_data_directory(data_folder_path: str):
    try:
        # Making the directory
        print("Data directory not found, creating...")
        os.makedirs(data_folder_path)

        # Creating the horses.json file
        print("Creating horses.json file...")
        with open(data_folder_path + "horses.json", "w") as file:
            file.write("[]")

        # Creating the keys.json file
        print("Creating keys.json file...")
        with open(data_folder_path + "keys.json", "w") as file:
            file.write('{"add_horse": "f1", "remove_horse": "f2", "barn_menu": "f3", "master_list": "f4"}')

        print("Data directory created successfully!")
        time.sleep(0.5)
    except Exception as e:
        print("Error while creating data directory:", e)
        input("Press Enter to close application...")
        exit(1)


class Data:
    horses: list[Horse]
    keys: dict[str, str]

    def __init__(self):
        self.horses = []
        self.keys = {}

    def _load_horses(self, file_path: str):
        with open(file_path, "r") as file:
            data: [] = json.loads(file.read())
            for horse in data:
                self.horses.append(Horse(horse["name"],
                                         # Casting to the UUID class
                                         UUID(horse["id"]),
                                         horse["is_trim"],
                                         horse["rotation_interval"],
                                         horse["last_shoe_date"]))

    def _load_keys(self, file_path: str):
        with open(file_path, "r") as file:
            self.keys = json.load(file)
        print("Loaded keys:", self.keys)

    def _save_horses(self, file_path: str):
        with open(file_path, "w") as file:
            file.write(self._stringify())

    def _save_keys(self, file_path: str):
        with open(file_path, "w") as file:
            file.write(json.dumps(self.keys))

    def _stringify(self) -> str:
        horse_data = []
        for horse in self.horses:
            horse_data.append({
                "name": horse.name,
                "id": horse.id,
                "is_trim": horse.is_trim,
                "rotation_interval": horse.rotation_interval,
                "last_shoe_date": horse.last_shoe_date.isoformat()
            })
        return json.dumps(horse_data)

    def save(self, file_path: str):
        with open(file_path, "w") as file:
            file.write(self._stringify())

    def load(self, data_folder_path: str):
        # Check if the path exists
        if not os.path.exists(data_folder_path):
            _generate_data_directory(data_folder_path)

        self._load_horses(data_folder_path + "horses.json")
        self._load_keys(data_folder_path + "keys.json")
        print("Data loaded successfully")
        time.sleep(0.5)

    def add_horse(self, horse: Horse):
        self.horses.append(horse)

    def remove_horse(self, horseName: str):
        for horse in self.horses:
            if horse.name == horseName:
                self.horses.remove(horse)
                break
        print(f"No horse by the name {horseName} exists")

    def get_horses_as_strings(self) -> list[str]:
        return [horse.name for horse in self.horses]

    def print_horses(self):
        for horse in self.horses:
            print(horse.name, horse.age, horse.is_trim, horse.rotation_interval, horse.last_shoe_date)

    def get_key(self, event_name: str):
        return self.keys.get(event_name)
