import React, { useEffect } from 'react'


interface MessagePopUp{
    message: string,
    type: string,
    show: boolean,
    onClose : () => void
}

const Error = ({message,type,show,onClose} : MessagePopUp) => {
    useEffect(() => {
        if(!message) return;
            const timer = setTimeout(() => onClose(),5000)

            return () => clearTimeout(timer); 
        
    },[show,onClose])
    if (!show) return null;
  return (
    <>
    <div
      className={`fixed top-0 left-1/2 -translate-x-1/2 mt-6 px-6 py-3 rounded-xl shadow-lg text-white ${
        type === 'Error' ? 'bg-red-500' : 'bg-green-500'
      }`}
    >
      {message}
    </div>
    </>
  )
}

export default Error