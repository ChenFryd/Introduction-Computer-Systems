@n1
D=M
@n2
D=D+M
@m
M=D

@counter
M=0

@finishedn1
M=0
@finishedn2
M=0

@n1counter
M=0

(STARTWHILE)
@a1
D=M
@n1counter
A=D+M
D=M
@tempLoc
M=D
@n2counter
D=M
@a2
A=M+D
D=M
@tempLoc
D=M-D
@n1lower
D;JLE

(n2lower)
@a2
D=M
@n2counter
A=D+M
D=M
@tempLoc
M=D
@counter
D=M
@a
D=M+D
@arrayCurrentLoc
M=D
@tempLoc
D=M
@arrayCurrentLoc
A=M
M=D

@counter
M=M+1

@n2counter
M=M+1

D=M
@n2
D=M-D
@CHECKEND
D;JNE
@finishedn2
M=1
@CHECKEND
0;JMP

(n1lower)
@a1
D=M
@n1counter
A=D+M
D=M
@tempLoc
M=D
@counter
D=M
@a
D=M+D
@arrayCurrentLoc
M=D
@tempLoc
D=M
@arrayCurrentLoc
A=M
M=D

@counter
M=M+1

@n1counter
M=M+1

D=M
@n1
D=M-D
@CHECKEND
D;JNE 
@finishedn1
M=1
@CHECKEND
0;JMP

(CHECKEND)
@m
D=M
@counter
D=D-M
@END
D;JEQ

@finishedn1
D=M
@n2lower
D;JGT 

@finishedn2
D=M
@n1lower
D;JGT 

@STARTWHILE
0;JMP

(END)