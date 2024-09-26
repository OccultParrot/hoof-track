import datetime

from src.horse import Horse


class Barn:
    name: str
    _horses: list[Horse]

    def __init__(self):
        self._horses = []

    def add_horse(self, horse: Horse):
        self._horses.append(horse)

    def remove_horse(self, horse: Horse):
        self._horses.remove(horse)

    def get_horses(self) -> list[Horse]:
        return self._horses
