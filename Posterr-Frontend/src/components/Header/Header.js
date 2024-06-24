import React, { useContext, useState } from "react";
import { FaHome, FaUser } from "react-icons/fa";
import { Link } from "react-router-dom";
import { UserContext } from "../../contexts/userContext";

import "./Header.css";

const Header = () => {
  const { users, selectedUser, updateUser } = useContext(UserContext);

  // Handle user change
  const handleUserChange = (event) => {
    const userId = event.target.value;
    updateUser(userId);
  };

  return (
    <div className="header-wrapper bg-[#292929]">
      <div className="header-container">
        <span className="poster-title pr-8">Posterr</span>

        <div className="header-buttons-container">
          <Link to={`/`} className="gap-2">
            <span className="text-md text-[#CCD6DD]">Home</span>
          </Link>
          <Link to={`/user/${selectedUser?.id}`} className="gap-2">
            <span className="text-md text-[#CCD6DD]">Profile</span>
          </Link>
        </div>

        <div className="header-user-container">
          <img
            src={selectedUser?.avatar || "/user_default.png"}
            alt="logged-user-avatar"
          />
          <select value={selectedUser?.id} onChange={handleUserChange}>
            {users.map((user) => (
              <option key={user.id} value={user.id}>
                {user.username}
              </option>
            ))}
          </select>
        </div>
      </div>
    </div>
  );
};

export default Header;
