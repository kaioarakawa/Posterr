import React from 'react';
import "./postForm.css";

const PostForm = ({ newPostContent, handleCreatePost, updateNewPostContent }) => {
    function handleTextChange(event) {
        updateNewPostContent(event.target.value);
    }

    const handlePostButtonClick = async () => {
        await handleCreatePost();
    }
    
    return (
    <div className="post-form-wrapper">
        <img src="/user_default.png" alt="logged-user-avatar" />

        <div className="post-form-container">
        <textarea
            placeholder="Start a post"
            rows={5}
            maxLength={777}
            value={newPostContent}
            onChange={handleTextChange}
        />

        <div className="post-form-button-container">
            <span>{newPostContent.length}/777</span>
            <button onClick={handlePostButtonClick} disabled={!newPostContent.trim().length}>
            <span>Post</span>
            </button>
        </div>
        </div>
    </div>
    );
};
    
export default PostForm;