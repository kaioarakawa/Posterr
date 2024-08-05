import React, { useEffect, useState } from "react";
import PersonIcon from "../common/personIcon/PersonIcon";
import { AiOutlineRetweet } from "react-icons/ai";
import { Link } from "react-router-dom";
import moment from "moment";
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
    <div key={postItem.id} className="post-card mb-2 ">
      <div className="post-content-container">
        <div className={postItem.originalPost ? "repost-info" : "post-info"}>
          {!!postItem.originalPost && !!postItem.content && (
            <div className="repost-info-container">
              <div className="flex items-center gap-1 mb-2">
                <PersonIcon size="sm" />
                <LinkToUserPage postItem={postItem}>
                  <span className="post-info-username">{`@${postItem.user.username}`}</span>
                </LinkToUserPage>
              </div>
              <span className="post-info-content">{postItem.content}</span>
            </div>
          )}

          {!!postItem.originalPost && !postItem.content && (
            <div className="repost-info-container flex items-center gap-1">
              <AiOutlineRetweet size={16} />
              <LinkToUserPage postItem={postItem}>
                <span>{postItem.user.name} reposted</span>
              </LinkToUserPage>
              <span className="post-info-content">{postItem.content}</span>
            </div>
          )}

          {!postItem.originalPost && (
            <div className="post-info-container">
              <div className="mb-2">
                <div className="flex gap-2 items-center">
                  {!postItem.originalPost && <PersonIcon size="sm" />}
                  <LinkToUserPage postItem={postItem}>
                    <span className="post-info-username">{`@${postItem.user.username}`}</span>
                  </LinkToUserPage>
                  <span className="post-info-date text-gray-500 text-sm">{`• ${moment(
                    postItem.createdAt
                  ).format("MMM DD, YYYY")}`}</span>
                </div>
              </div>

              <span className="post-info-content">{postItem.content}</span>
            </div>
          )}

          {postItem.originalPost && (
            <div
              className={`sub-post-container border p-2 rounded-md border-gray-500 my-4 ${
                postItem.content ? "ml-8" : ""
              }`}
            >
              <Post post={postItem.originalPost} />
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default Post;
