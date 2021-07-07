select tp.TeacherId 
from Pupil as p
inner join TeacherPupil as tp
on p.PupilId = tp.PupilId
where p.PupilName = 'giorgi'