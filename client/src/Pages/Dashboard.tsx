//import React from "react";

import { Button } from "../Components/ui/button";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "../Components/ui/popover";
import { useNavigate } from "react-router-dom";
//import { useIsMobile } from "../Hooks/userIsMobile";
import { useDispatch } from "react-redux";
import type { AppDispatch } from "../redux/store";
import {  signOut } from "../redux/authSlice";
const Dashboard = () => {
  //const isMobile = useIsMobile();
  const dispatch = useDispatch<AppDispatch>(); 
  const navigate = useNavigate();
  return (
    <>
      <nav className="w-full flex item-center justify-between px-6 py-4 shadow-md bg-white ">
        <div className="flex item-center space-x-2">
          <img
            src=""
            alt="Financial Tracker"
            className="h-10 w-10 object-contain"
          />
        </div>
        <Popover>
          <PopoverTrigger asChild>
            <img src="" alt="per" className="h-10 w-10 object-contain" />
          </PopoverTrigger>
          <PopoverContent className="bg-white flex flex-col items-center justify-center gap-2 p-4 shadow-lg">
            <img src="" alt="per" className="h-10 w-10 object-contain" />
            <a
              href="/Profile"
              className="w-full text-center py-2 px-4 rounded hover:shadow-lg hover:bg-gray-100 transition-all"
            >
              Your profile
            </a>
            <a
              href="/Settings"
              className="w-full text-center py-2 px-4 rounded hover:shadow-lg hover:bg-gray-100 transition-all"
            >
              Settings
            </a>
            <Button
              className="w-full py-2 px-4 bg-red-500 text-white rounded hover:shadow-lg hover:bg-red-600 transition-all"
              onClick={async ()=> {
                await dispatch(signOut()).then(() => navigate('/'));
              }}
            >
              Log out
            </Button>
          </PopoverContent>
        </Popover>
      </nav>
    </>
  );
};

export default Dashboard;
