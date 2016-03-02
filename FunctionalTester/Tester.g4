grammar Tester;

@parser::members
{
    protected const int EOF = Eof;
}
 
@lexer::members
{
    protected const int EOF = Eof;
    protected const int HIDDEN = Hidden;
}

prog : NewlineLiteral* topLevel+ EOF;

topLevel :  OpenBrace Identifier CloseBrace NewlineLiteral functionBody #FunctionTop
	| OpenBracket Identifier CloseBracket EqualOp NewlineLiteral* expr NewlineLiteral+ #AssignmentTop
	;

functionBody : (statement)+ ;

statement : Identifier EqualOp expr NewlineLiteral+ # AssignStatement
	| AssertToken expr NewlineLiteral+ #AssertStatement
	| expr NewlineLiteral+ #ExprStatement
;

expr: Identifier #IdentExpr
	| IntegerLiteral #IntExpr
	| BooleanLiteral #BoolExpr
	| MultilineLiteral #MultilineExpr
	| StringLiteral  #StringExpr
	| EqualToken expr expr #EqualExpr
	| OutputToken expr #OutputExpr
	| WaitToken expr+ #WaitExpr
	| RunToken expr expr? #RunExpr
	| ShellToken expr #ShellExpr
	| OpenParen expr CloseParen #ParenExpr
	| ConnectToken expr expr? #ConnectExpr
	| DisconnectToken expr expr? #DisconnectExpr
	| SSHToken expr expr #SshExpr
	| SCPToken expr expr expr? #ScpExpr
	| BangOp expr #NotExpr
	| OSToken expr expr #OsExpr
	| ReadToken expr #ReadExpr
	| WriteToken expr expr #WriteExpr
;

WhiteSpace: [ \t]+ -> channel(HIDDEN);

AssertToken: 'assert' ;
OutputToken: 'output' ;
WaitToken: 'wait' ;
EqualToken: 'equal' ;
RunToken: 'run' ;
ShellToken: 'shell' ;
ConnectToken: 'connect' ;
DisconnectToken: 'disconnect' ;
SSHToken: 'ssh' ;
SCPToken: 'scp' ;
OSToken: 'osswitch' ;
ReadToken: 'read' ;
WriteToken: 'write' ;

OpenParen: '(' ;
CloseParen: ')' ;
OpenBrace: '[' ;
CloseBrace: ']' ;
OpenBracket: '{' ;
CloseBracket: '}' ;

EqualOp : '=' ;
BangOp : '!' ;

BooleanLiteral : 'true' | 'false' ;
Identifier : [_a-zA-Z][_A-Za-z0-9]* ;
IntegerLiteral : [0-9]+ ;

MultilineLiteral : '"""' .*? '"""' ;
fragment NEWLINE_CORE : ('\r\n' | '\n') ;
fragment ESCAPED_QUOTE : '\\"';
StringLiteral :   '"' ( ESCAPED_QUOTE | ~('\r' | '\n') )*? '"' ;

NewlineLiteral : NEWLINE_CORE;

