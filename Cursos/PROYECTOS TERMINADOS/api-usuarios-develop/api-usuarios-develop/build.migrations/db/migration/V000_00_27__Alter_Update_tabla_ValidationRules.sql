  ALTER TABLE [api_usuarios].[dbo].[ValidationRules] ALTER COLUMN [RegularExpression] varchar(1000)

  UPDATE [api_usuarios].[dbo].[ValidationRules]
  SET RegularExpression = '/^(?!.*(?:0123|1234|2345|3456|4567|5678|6789|9876|8765|7654|6543|5432|4321|3210|abcd|bcde|cdef|defg|efgh|fghi|ghij|hijk|ijkl|jklm|klmn|lmno|mnop|nopq|opqr|pqrs|qrst|rstu|stuv|tuvw|uvwx|vwxy|wxyz|zyxw|yxwv|xwvu|wvut|vuts|utsr|tsru|srqp|rqpo|qpon|ponm|onml|nmlk|mlkj|lkji|kjih|jihg|ihgf|hgfe|gfed|fedc|edcb|dcba|ABCD|BCDE|CDEF|DEFG|EFGH|FGHI|GHIJ|HIJK|IJKL|JKLM|KLMN|LMNO|MNOP|NOPQ|OPQR|PQRS|QRST|RSTU|STUV|TUVW|UVWX|VWXY|WXYZ|ZYXW|YXWV|XWVU|WVUT|VUTS|UTSR|TSRU|SRQP|RQPO|QPON|PONM|ONML|NMLK|MLKJ|LKJI|KJIH|JIHG|IHGF|HGFE|GFED|FEDC|EDCB|DCBA)).+$/i'
  WHERE ValidationRuleName = 'NoMasDe3CaracteresConsecutivosAscODesc'