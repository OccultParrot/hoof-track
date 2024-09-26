from src.horse import Horse
import json
import time
import os


def _generate_data_directory(data_folder_path: str):
    try:
        # Making the directory
        print("Data directory not found, creating...")
        os.makedirs(data_folder_path)

        # Creating the horses.json file
        print("Creating horses.json file...")
        with open(data_folder_path + "horses.json", "w") as file:
            file.write("[]")

        print("Data directory created successfully!")
        time.sleep(0.5)

    except Exception as e:
        print("Error while creating data directory:", e)
        input("Press Enter to close application...")
        exit(1)


class Data:
    horses: list[Horse]

    def __init__(self):
        self.horses = []

    def _load_horses(self, file_path: str):
        with open(file_path, "r") as file:
            data: [] = json.loads(file.read())
            for horse in data:
                self.horses.append(Horse(horse["name"],
                                         horse["id"],
                                         horse["age"],
                                         horse["is_trim"],
                                         horse["rotation_interval"],
                                         horse["last_shoe_date"]))

    def _stringify(self) -> str:
        horse_data = []
        for horse in self.horses:
            horse_data.append({
                "name": horse.name,
                "id": horse.id,
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
            _generate_data_directory(data_folder_path)

        self._load_horses(data_folder_path + "horses.json")
        print("Data loaded successfully")
        time.sleep(0.5)

    def add_horse(self, horse: Horse):
        self.horses.append(horse)

    def remove_horse(self, horse: Horse):
        self.horses.remove(horse)

    def print_horses(self):
        for horse in self.horses:
            print(horse.name, horse.age, horse.is_trim, horse.rotation_interval, horse.last_shoe_date)
