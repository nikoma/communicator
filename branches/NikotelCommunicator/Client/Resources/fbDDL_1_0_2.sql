
DROP PROCEDURE MESSAGE_ADD;
DROP PROCEDURE MESSAGE_FIND;
DROP PROCEDURE MESSAGE_ARCHIVE_USERS;


ALTER TABLE MESSAGES ADD CONTENT_HTML Varchar(6144) CHARACTER SET UTF8 COLLATE UTF8;
ALTER TABLE MESSAGES ADD JID Varchar(64) CHARACTER SET UTF8 COLLATE UTF8;
ALTER TABLE MESSAGES ALTER CONTENT TO CONTENT_TEXT;


UPDATE MESSAGES SET CONTENT_HTML = CONTENT_TEXT;
UPDATE MESSAGES SET JID = USERNAME || '@im.nikotel.com';

SET TERM ^ ;
CREATE PROCEDURE MESSAGE_ADD (
    iUSER_ID Integer,
    iGUID Varchar(128) CHARACTER SET UTF8,
    iJID Varchar(64) CHARACTER SET UTF8,
    iCONTENT_TEXT Varchar(4096) CHARACTER SET UTF8,
	iCONTENT_HTML Varchar(6144) CHARACTER SET UTF8,
    iDIRECTION Varchar(8) )
AS
BEGIN EXIT; END^
SET TERM ; ^

SET TERM ^ ;
ALTER PROCEDURE MESSAGE_ADD (
    iUSER_ID Integer,
    iGUID Varchar(128) CHARACTER SET UTF8,
    iJID Varchar(64) CHARACTER SET UTF8,
    iCONTENT_TEXT Varchar(4096) CHARACTER SET UTF8,
	iCONTENT_HTML Varchar(6144) CHARACTER SET UTF8,
    iDIRECTION Varchar(8) )
AS
BEGIN 
 BEGIN 
 INSERT INTO MESSAGES (USER_ID, GUID,JID,CONTENT_TEXT, CONTENT_HTML, DIRECTION) VALUES (:iUSER_ID, :iGUID,lower(:iJID),:iCONTENT_TEXT,:iCONTENT_HTML,:iDIRECTION); 
 END 
 SUSPEND; 
END^
SET TERM ; ^


SET TERM ^ ;
CREATE PROCEDURE MESSAGE_FIND (
    iUSER_ID Integer,
    iJID Varchar(64) CHARACTER SET UTF8,
    iSEARCH Varchar(64) CHARACTER SET UTF8,
    iLIMIT Integer)
RETURNS (
    oJID Varchar(64) CHARACTER SET UTF8,
    oCONTENT_TEXT Varchar(4096) CHARACTER SET UTF8,
    oCONTENT_HTML Varchar(6144) CHARACTER SET UTF8,
    oDIRECTION Varchar(8) CHARACTER SET UTF8,
    oCREATED Timestamp )
AS
BEGIN EXIT; END^
SET TERM ; ^

SET TERM ^ ;
ALTER PROCEDURE MESSAGE_FIND (
    iUSER_ID Integer,
    iJID Varchar(64) CHARACTER SET UTF8,
    iSEARCH Varchar(64) CHARACTER SET UTF8,
    iLIMIT Integer )
RETURNS (
    oJID Varchar(64) CHARACTER SET UTF8,
	oCONTENT_TEXT Varchar(4096) CHARACTER SET UTF8,
    oCONTENT_HTML Varchar(6144) CHARACTER SET UTF8,
    oDIRECTION Varchar(8) CHARACTER SET UTF8,
    oCREATED Timestamp )
AS
BEGIN 

FOR SELECT FIRST (:iLIMIT) JID, CONTENT_TEXT, CONTENT_HTML, DIRECTION, CREATED 
FROM MESSAGES 
WHERE LOWER(CONTENT_TEXT) like LOWER(:iSEARCH) 
AND lower(JID)  = lower(:iJID)
AND USER_ID =  :iUSER_ID 
ORDER BY CREATED DESC 
INTO :oJID, :oCONTENT_TEXT,oCONTENT_HTML, :oDIRECTION , :oCREATED DO
  BEGIN
   SUSPEND;
  END

END^
SET TERM ; ^

SET TERM ^ ;
CREATE PROCEDURE MESSAGE_ARCHIVE_USERS (
    iUSER_ID Integer
)
RETURNS (
    oJID Varchar(64) CHARACTER SET UTF8,
    oDIRECTION Varchar(8) CHARACTER SET UTF8,
    oCREATED Timestamp )
AS
BEGIN EXIT; END^
SET TERM ; ^


SET TERM ^ ;
ALTER PROCEDURE MESSAGE_ARCHIVE_USERS (
    iUSER_ID Integer
)
RETURNS (
    oJID Varchar(64) CHARACTER SET UTF8,
    oDIRECTION Varchar(8) CHARACTER SET UTF8,
    oCREATED Timestamp )
AS
BEGIN 
FOR SELECT DISTINCT JID, DIRECTION,  CREATED
FROM MESSAGES M1
WHERE USER_ID =  :iUSER_ID AND CREATED = 
( SELECT MAX (CREATED) FROM MESSAGES M2
WHERE M1.JID = M2.JID
)
ORDER BY JID DESC 
INTO :oJID, :oDIRECTION , :oCREATED DO
  BEGIN
   SUSPEND;
  END
END^
SET TERM ; ^


GRANT EXECUTE
 ON PROCEDURE MESSAGE_FIND TO  SYSDBA;

GRANT EXECUTE
 ON PROCEDURE MESSAGE_ADD TO  SYSDBA;
 
 SET TERM ^ ;
ALTER PROCEDURE OPENDB (
    iUSERNAME Varchar(64) CHARACTER SET UTF8 )
RETURNS (
    oUSER_ID Integer,
    oVERSION Varchar(32) CHARACTER SET UTF8 )
AS
DECLARE VARIABLE cnt INTEGER; 
BEGIN 
 SELECT count(USER_ID) FROM USERS WHERE USERNAME = :iUSERNAME INTO :cnt; 
 IF (cnt = 0) THEN 
  INSERT INTO USERS (USERNAME) VALUES (:iUSERNAME); 
 FOR SELECT USER_ID , '1.0.2'
  FROM USERS 
  WHERE USERNAME = :iUSERNAME 
  INTO :oUSER_ID , :oVERSION
  DO SUSPEND; 
END^
SET TERM ; ^
GRANT EXECUTE
 ON PROCEDURE OPENDB TO  SYSDBA;
 
 ALTER TABLE MESSAGES DROP USERNAME;