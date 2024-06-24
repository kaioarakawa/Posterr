import React, { createContext, useState, useEffect } from "react";
import api from "../services/api";

const UserContext = createContext();

const defaultUser = {
  id: 0,
  name: null,
  username: null,
};

const UserProvider = ({ children }) => {
  const [users, setUsers] = useState([]);
  const [selectedUser, setSelectedUser] = useState(defaultUser);

  useEffect(() => {
    fetchUsers();
  }, []);

  const fetchUsers = async () => {
    try {
      const response = await api.get(`/users`);
      setUsers(response.data);

      // Select the first user by default
      if (response.data.length > 0) {
        setSelectedUser(response.data[0]);
      }
    } catch (error) {
      console.error("Error fetching users:", error);
    }
  };

  const updateUser = (userId) => {
    const user = users.find((user) => user.id === userId);
    if (user) {
      setSelectedUser(user);
    }
  };

  return (
    <UserContext.Provider value={{ users, selectedUser, updateUser }}>
      {children}
    </UserContext.Provider>
  );
};

export { UserContext, UserProvider };
