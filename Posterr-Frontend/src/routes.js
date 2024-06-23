import React, { useContext } from "react";
import { Route, Routes } from "react-router-dom";
// import { Post } from "./components";
import {Home, Profile} from "./pages";

const Router = () => {

  return (
    <Routes>
      <Route path="/" >
        <Route path="/" element={<Home />} />
        <Route path="/user/:id" element={<Profile />} />
      </Route>
      <Route path="*" element={<h1>Invalid route</h1>} />
    </Routes>
  );
};

export default Router;
