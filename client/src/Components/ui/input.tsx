import * as React from "react"

import { cn } from "@/lib/utils"

interface InputProps extends React.ComponentProps<"input">{
  errorMessage? : string 
} 

const Input = React.forwardRef<HTMLInputElement, InputProps>(
  ({ className, type,required, errorMessage = "Required", ...props }, ref) => {
   const [error,setError] = React.useState("");

   const handleInvalid = (e: React.FormEvent<HTMLInputElement>)=>{
    e.preventDefault();
    setTimeout(() => {
      setError("")
    }, 2000);
    setError(errorMessage);
   }
   const handleChange = ()=>{
    setError("");
   }
    return (
      <div className="w-full">
      <input
        type={type}
        required = {required}
        className={cn(
          "flex h-9 w-full rounded-md border border-input bg-transparent px-3 py-1 text-base shadow-sm transition-colors file:border-0 file:bg-transparent file:text-sm file:font-medium file:text-foreground placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring disabled:cursor-not-allowed disabled:opacity-50 md:text-sm",
          className
        )}
        ref={ref}
        onInvalid = {handleInvalid}
        onChange={handleChange}
        {...props}
      />
      {error && <p className="text-red-600 text-sm mt-1">{error}</p>}
      </div>
    )
  }
)
Input.displayName = "Input"

export { Input }
