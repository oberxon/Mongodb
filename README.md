# Mongodb
perform normal mongodb operations using configuration file.

convert general tsql read query to mongodb query and execute.


exp:

tsql query

      select * from total where value=5 or value in (6,7,8) and _id like '18%' or value >56 or value<=188 order by value offset 10 rows fetch next 3  rows only


mongodb query

      db.total.find({$or : [{$or : [{$or : [{value : { $eq : 5 }}, {value : { $in : [6, 7, 8]}, _id : {$regex: '^1'}}]}, {value : { $gt : 56}}]}, {value : { $lte : 188}}]}).sort({value:1}).limit(3).skip(10)

