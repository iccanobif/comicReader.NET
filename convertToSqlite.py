import sqlite3
import uuid

c = sqlite3.connect("comicLibrary.db")
c.execute("CREATE TABLE IF NOT EXISTS COMICS (GBL_ID, PATH, TITLE, POSITION)")
c.execute("delete from comics")
f = open("position.lst")
lines = f.readlines()

i = 0

while i < len(lines):
	title = lines[i].strip()
	i+=1
	path  = lines[i].strip()
	i+=1
	position = int(lines[i].strip()) - 1
	i+=1
	c.execute("insert into comics (gbl_id, title, path, position) values (?, ?, ?, ?)", (str(uuid.uuid1()), title, path, position))


c.commit()
c.close()