import React, { useContext, useState } from "react";
import { FaHome, FaUser } from "react-icons/fa";
import { Link } from "react-router-dom";
import { UserContext } from "../../contexts/userContext";

import "./header.css";

const Header = () => {
    const { users, selectedUser, updateUser } = useContext(UserContext);

    // Handle user change
    const handleUserChange = (event) => {
        const userId = event.target.value;
        updateUser(userId);
    };

    return (
        <div className="header-wrapper">
            <div className="header-container">
                <span className="poster-title">Posterr</span>

                <div className="header-buttons-container">
                    <Link to={`/`}>
                        <FaHome size={24} />
                        <span>Home</span>
                    </Link>
                    <Link to={`/user/${selectedUser?.id}`}>
                        <FaUser size={24} />
                        <span>Profile</span>
                    </Link>
                </div>

                <div className="header-user-container">
                    <img src={selectedUser?.avatar || '/user_default.png'} alt="logged-user-avatar" />
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
