

// Draws a square at the top-left corner of the screen.
// The square is R0 chars wide and R0 chars high.

   D=x
   D;JLE:INFINITE_LOOP 
   j=x
   @SCREEN
   D=A
   row_address=D

(OUTER_LOOP)
   address=row_address
   i=x	

(INNER_LOOP)
   @88
   D=A
   A=address
   M=D
   address++
   i--
   D=i
   D;JGT:INNER_LOOP

   @80
   D=A
   A=row_address
   D=D+A
   row_address=D

   j--
   D=j
   D;JGT:OUTER_LOOP

(INFINITE_LOOP)
   0;JMP:INFINITE_LOOP
