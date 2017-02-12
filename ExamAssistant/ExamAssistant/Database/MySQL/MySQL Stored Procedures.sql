DELIMITER $$
CREATE PROCEDURE `VerifyUser`(IN userUsername VARCHAR(45), 
IN userPassword VARCHAR(150))
BEGIN
	SELECT * FROM USERS u WHERE USERNAME = userUsername AND BINARY password = userPassword AND is_active = 1;
END$$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE `GetUserDetails`(IN userUsername VARCHAR(45))
BEGIN
		SELECT
		U.USER_ID,
		U.USERNAME,
		U.FIRST_NAME,
		U.LAST_NAME,
		U.GRADE_LEVEL,
		U.SECTION,
        U.IS_ACTIVE,
        U.IS_ADMIN,
        CASE WHEN C.USERNAME IS NULL THEN 0 ELSE C.USERNAME END AS 'CREATED_BY',
        U.CREATED_DATE,
        CASE WHEN UP.USERNAME IS NULL THEN 0 ELSE UP.USERNAME END AS 'UPDATED_BY',
        U.UPDATED_DATE
    FROM USERS U
    LEFT JOIN USERS C
		ON C.user_id = U.created_by
	LEFT JOIN USERS UP
		ON UP.USER_ID = U.updated_by
    WHERE U.USERNAME = userUsername;
END$$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE `GetStudentList`()
BEGIN
	SELECT
		U.USER_ID,
		U.USERNAME,
		U.FIRST_NAME,
		U.LAST_NAME,
		U.GRADE_LEVEL,
		U.SECTION,
        U.IS_ACTIVE,
		C.USERNAME AS 'CREATED BY',
        U.CREATED_DATE,
		UP.USERNAME AS 'UPDATED BY',
        U.UPDATED_DATE
	FROM USERS U
	INNER JOIN USERS C
		ON C.user_id = U.created_by
	INNER JOIN USERS UP
		ON UP.USER_ID = U.updated_by
	WHERE U.IS_ADMIN = 0
    ORDER BY U.GRADE_LEVEL, U.USERNAME;
END$$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE `GetExamList`()
BEGIN
	SELECT 
		E.EXAM_ID,
		E.NAME AS 'EXAM NAME',
		S.NAME AS 'SUBJECT NAME',
		ET.NAME AS 'EXAM TYPE',
        SUM(I.points) AS 'TOTAL POINTS',
		U1.USERNAME AS 'CREATED BY',
		E.CREATED_DATE,
		U2.USERNAME AS 'UPDATED BY',
		E.UPDATED_DATE
	FROM EXAMS E
    INNER JOIN exam_sections ES
		ON E.EXAM_ID = ES.exam_id
	INNER JOIN exam_items I
		ON ES.exam_section_id = I.exam_section_id
	INNER JOIN SUBJECTS S
		ON E.subject_id = S.subject_id
	INNER JOIN exam_types ET
		ON E.exam_type_id = ET.exam_type_id
	INNER JOIN USERS U1
		ON E.created_by = U1.user_id
	INNER JOIN USERS U2
		ON e.updated_by = U2.user_id
	GROUP BY EXAM_ID;

END$$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE `DeleteExam`(IN examId INT(11))
BEGIN
	DELETE FROM EXAMS WHERE EXAM_ID = examId;
END$$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE `DeactivateUser`(IN userUserId INT(11))
BEGIN
	UPDATE USERS SET IS_ACTIVE = 0 WHERE USER_ID = userUserId;
END$$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE `AddUser`(
	userUsername varchar(45), 
	userPassword VARCHAR(150),
	userFirstname VARCHAR(100),
	userLastname VARCHAR(100), 
	userGradeLevel int(11), 
	userSection VARCHAR(45), 
	userIsAdmin INT(11),
	userCreatedBy int(11), 
	userUpdatedBy int(11)
)
BEGIN
INSERT INTO USERS (username, password, first_name, 
last_name, grade_level, section, is_admin, is_active, created_by, updated_by)
VALUES (userUsername, userPassword, userFirstname, 
userLastname, userGradeLevel, userSection, userIsAdmin, 1, userCreatedBy, userUpdatedBy);
END$$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE `ActivateUser`(IN userUserId INT(11))
BEGIN
	UPDATE USERS SET IS_ACTIVE = 1 WHERE USER_ID = userUserId AND IS_ACTIVE = 0;
END$$
DELIMITER ;
