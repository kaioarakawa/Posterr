import React, { useEffect, useState } from "react";
import { AiOutlineRetweet } from "react-icons/ai";
import { Link } from "react-router-dom";
// import { format } from "date-fns";
// import { usePosts } from "../../hooks";
import "./post.css";

const LinkToUserPage = ({ postItem, children }) => (
  <Link to={`/user/${postItem.user.id}`}>{children}</Link>
);

const Post = ({ post }) => {
  const [postItem, setPostItem] = useState(null);

  useEffect(() => {
    setPostItem(post);
  }, [post]);

  if (!postItem) return null;

  return (
    <div key={postItem.id} className="post-card">
      <div className="post-content-container">
        {!postItem.originalPost && (
          <img src="/user_default.png" alt="logged-user-avatar" />
        )}

        <div className={postItem.originalPost ? "repost-info" : "post-info"}>
          {postItem.originalPost ? (
            <div className="repost-info-container">
              <AiOutlineRetweet size={20} />
              <LinkToUserPage postItem={postItem}>
                <span>{postItem.user.name}</span>
              </LinkToUserPage>
              <span>Reposted</span>
              <span className="post-info-date">{`• ${new Date(postItem.createdAt)}`}</span>
              <span className="post-info-content">{postItem.content}</span>
            </div>
          ) : (
            <div className="post-info-container">
              <div>
                {/* <span className="post-info-name">{postItem.user.name}</span> */}
                <LinkToUserPage postItem={postItem}>
                  <span className="post-info-username">{`@${postItem.user.username}`}</span>
                </LinkToUserPage>
                <span className="post-info-date">{`• ${new Date(postItem.createdAt)}`}</span>
              </div>

              <span className="post-text">{postItem.content}</span>
            </div>

          )}

          {postItem.originalPost && (
            <div className="sub-post-container">
              <Post post={postItem.originalPost} />
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default Post;
