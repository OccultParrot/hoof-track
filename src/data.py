from src.horse import Horse
import json
import time
import os


class Data:
    horses: list[Horse]

    def __init__(self):
        self.horses = []

    def _load_horses(self, file_path: str):
        with open(file_path, "r") as file:
            data: [] = json.loads(file.read())
            for horse in data:
                self.horses.append(Horse(horse["name"],
                                         horse["age"],
                                         horse["is_trim"],
                                         horse["rotation_interval"],
                                         horse["last_shoe_date"]))

    def _stringify(self) -> str:
        horse_data = []
        for horse in self.horses:
            horse_data.append({
                "name": horse.name,
                "age": horse.age,
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
            raise Exception("Data folder does not exist")

        self._load_horses(data_folder_path + "horses.json")
        print("Data loaded successfully")
        time.sleep(0.5)

    def print_horses(self):
        for horse in self.horses:
            print(horse.name, horse.age, horse.is_trim, horse.rotation_interval, horse.last_shoe_date)
