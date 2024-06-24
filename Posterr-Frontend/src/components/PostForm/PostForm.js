import React from "react";
import Button from "../common/button/Button";
import PersonIcon from "../common/personIcon/PersonIcon";
import "./PostForm.css";

const PostForm = ({
  newPostContent,
  handleCreatePost,
  updateNewPostContent,
}) => {
  function handleTextChange(event) {
    updateNewPostContent(event.target.value);
  }

  const handlePostButtonClick = async () => {
    await handleCreatePost();
  };

  return (
    <div className="post-form-wrapper">
      <PersonIcon size="md" />

      <div className="post-form-container">
        <textarea
          placeholder="Start a post"
          rows={5}
          maxLength={777}
          value={newPostContent}
          onChange={handleTextChange}
          className="border-black border rounded-md p-2 border-gray-500 bg-[#0c0c0c] text-gray-500"
        />

        <div className="post-form-button-container">
          <span>{newPostContent.length}/777</span>
          <Button
            text="Post"
            action={handlePostButtonClick}
            disabled={!newPostContent.trim().length}
          />
        </div>
      </div>
    </div>
  );
};

export default PostForm;
