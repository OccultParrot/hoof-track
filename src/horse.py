import datetime



class Horse:
    id: int
    name: str
    age: int
    is_trim: bool
    rotation_interval: int
    last_shoe_date: datetime.date

    def __init__(self, name: str, horse_id: int, age: int, is_trim: bool, rotation_interval: int, last_shoe_date: datetime.date):
        self.name = name
        self.id = horse_id
        self.age = age
        self.is_trim = is_trim
        self.rotation_interval = rotation_interval
        self.last_shoe_date = last_shoe_date

    def reset(self, date: datetime.date):
        self.last_shoe_date = date

    def get_weeks_overdue(self) -> int:
        return (datetime.date.today() - self.last_shoe_date).days // 7 - self.rotation_interval
