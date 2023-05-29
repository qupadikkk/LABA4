# LABA4
Додано клас TransportMileageEvantArgs, який розширює EventArgs і містить властивість KmCount для передачі відстані події, додано клас TransportSTO, який містить метод OverComingTheNext10Km для перевірки, чи потрібна перевірка СТО при подоланні наступних 10 000 км.
Змінено клас Transport, додано делегат MileageEventHandler і подію MileageChanged, які використовуються для сповіщення про зміну пробігу транспорту
