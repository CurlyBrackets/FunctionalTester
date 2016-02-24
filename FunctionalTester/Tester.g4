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

prog : topLevel+ EOF;

topLevel :  OpenBrace Identifier CloseBrace NewlineLiteral functionBody #FunctionTop
	| OpenBracket Identifier CloseBracket NewlineLiteral multilineStringR #MultilineTop
	;

functionBody : (statement)+ ;

statement : Identifier EqualOp expr NewlineLiteral # AssignStatement
	| AssertToken expr NewlineLiteral #AssertStatement
	| expr NewlineLiteral #ExprStatement
;

expr: Identifier #IdentExpr
	| IntegerLiteral #IntExpr
	| BooleanLiteral #BoolExpr
	| StringLiteral  #StringExpr
	| EqualToken expr expr #EqualExpr
	| OutputToken expr #OutputExpr
	| WaitToken expr+ #WaitExpr
	| RunToken expr expr? #RunExpr
	| ShellToken expr #ShellExpr
	| OpenParen expr CloseParen #ParenExpr
;

multilineStringR : MultilineElement NewlineLiteral #MultilineString
;

WhiteSpace: [ \t]+ -> channel(HIDDEN);

AssertToken: 'assert' ;
OutputToken: 'output' ;
WaitToken: 'wait' ;
EqualToken: 'equal' ;
RunToken: 'run' ;
ShellToken: 'shell' ;

OpenParen: '(' ;
CloseParen: ')' ;
OpenBrace: '[' ;
CloseBrace: ']' ;
OpenBracket: '{' ;
CloseBracket: '}' ;

EqualOp : '=' ;

BooleanLiteral : 'true' | 'false' ;
Identifier : [_a-zA-Z][_A-Za-z0-9]* ;
IntegerLiteral : [0-9]+ ;


fragment NEWLINE_CORE : ('\r\n' | '\n');
fragment ESCAPED_QUOTE : '\\"';
StringLiteral :   '"' ( ESCAPED_QUOTE | ~('\r' | '\n') )*? '"';

NewlineLiteral : NEWLINE_CORE;
MultilineElement : ~('{' | '[') ;