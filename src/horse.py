from uuid import uuid4 as uuid, UUID
import datetime

def new_horse(name: str, is_trim: bool, rotation_interval:int, last_shoe_date: datetime.date):
    """
    Use this to generate a new Horse object, **UNLESS** reading from database, cause then it won't be missing the id

    :returns Horse
    """
    return Horse(name, uuid(), is_trim, rotation_interval, last_shoe_date)

class Horse:
    id: UUID
    name: str
    is_trim: bool
    rotation_interval: int
    last_shoe_date: datetime.date

    def __init__(self, name: str, horse_id: UUID, is_trim: bool, rotation_interval: int, last_shoe_date: datetime.date):
        self.name = name
        self.id = horse_id
        self.is_trim = is_trim
        self.rotation_interval = rotation_interval
        self.last_shoe_date = last_shoe_date

    def reset(self, date: datetime.date):
        self.last_shoe_date = date

    def get_weeks_overdue(self) -> int:
        return (datetime.date.today() - self.last_shoe_date).days // 7 - int(self.rotation_interval)

    def __str__(self):
        return self.name
