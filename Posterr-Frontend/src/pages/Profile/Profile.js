import React, {
  useState,
  useEffect,
  useContext,
  useCallback,
  useRef,
} from "react";
import api from "../../services/api";
import { VscCalendar } from "react-icons/vsc";
import PostForm from "../../components/PostForm/PostForm";
import Header from "../../components/Header/Header";
import Post from "../../components/Post/Post";
import { UserContext } from "../../contexts/userContext";
import { useParams } from "react-router-dom";
import {
  showSuccessMessage,
  showErrorMessage,
} from "../../utils/sweetalertUtils";

import "./profile.css";

const Profile = () => {
  const defaultUser = {
    id: 0,
    name: null,
    username: null,
  };
  const { id } = useParams();
  const [posts, setPosts] = useState([]);
  const [newPostContent, setNewPostContent] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [filter, setFilter] = useState("");
  const [sortOrder, setSortOrder] = useState("latest");
  const [profileUser, setProfileUser] = useState(defaultUser);
  const { selectedUser } = useContext(UserContext);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPosts, setTotalPosts] = useState(0);
  const observer = useRef();

  const getUserById = useCallback(async () => {
    setLoading(true);
    try {
      const res = await api.get(`/users/${id}`);
      setProfileUser(res.data);
      setLoading(false);
    } catch (error) {
      setError(error.message);
      setLoading(false);
    }
  }, [id]);

  useEffect(() => {
    if (id) {
      getUserById();
    }
  }, [id, getUserById]);

  useEffect(() => {
    if (profileUser.id !== 0) {
      fetchPosts(1, 10);
    }
  }, [profileUser.id, sortOrder, filter]);

  const fetchPosts = async (page, take) => {
    setLoading(true);
    try {
      const res = await api.get("/posts", {
        params: {
          skip: (page - 1) * take,
          take: take,
          sortBy: sortOrder,
          keyword: filter,
          userId: profileUser.id,
        },
      });
      setPosts((prevPosts) =>
        page === 1 ? res.data.posts : [...prevPosts, ...res.data.posts]
      );
      setCurrentPage(page);
      setTotalPosts(res.totalPosts);
      setLoading(false);
    } catch (error) {
      setError(error.message);
      setLoading(false);
      showErrorMessage(`Failed to fetch posts: ${error.message}`);
    }
  };

  const handleCreatePost = async () => {
    const post = { content: newPostContent, userId: selectedUser.id };
    try {
      await api.post("/posts", post);
      setNewPostContent("");
      showSuccessMessage("Post created successfully!");
      fetchPosts(1, posts.length + 1);
    } catch (error) {
      showErrorMessage(`Failed to create post: ${error.response.data}`);
    }
  };

  // Infinite scrolling observer
  const lastPostElementRef = useCallback(
    (node) => {
      if (loading) return;
      if (observer.current) observer.current.disconnect();
      observer.current = new IntersectionObserver((entries) => {
        if (entries[0].isIntersecting && posts.length < totalPosts) {
          fetchPosts(currentPage + 1, 10);
        }
      });
      if (node) observer.current.observe(node);
    },
    [loading, currentPage, totalPosts]
  );

  return (
    <div>
      <Header />
      <div className="px-12 pb-8 pt-20 text-gray-500">
        <div className="user-page-container">
          <div className="flex gap-8 mb-8">
            <img
              src="/user_default.png"
              alt="logged-user-avatar"
              className="rounded-sm w-32"
            />

            <div className="user-data-container-2">
              <div className="user-username-container">
                <span className="user-info-description font-bold text-4xl text-[#ccd6dd]">
                  {profileUser.name}
                </span>
              </div>

              <span className="user-name text-sm">{`@${profileUser.username}`}</span>
              <span className="user-info-description flex gap-2 items-center text-sm">
                <VscCalendar />
                {` Joined ${new Date(
                  profileUser.createdAt
                ).toLocaleDateString()}`}
              </span>
            </div>
          </div>
        </div>

        {profileUser && selectedUser && profileUser.id === selectedUser.id ? (
          <div className="user-post-form-container">
            <PostForm
              newPostContent={newPostContent}
              handleCreatePost={handleCreatePost}
              updateNewPostContent={setNewPostContent}
            />
          </div>
        ) : null}

        <div className="user-posts-container">
          {loading && posts.length === 0 ? (
            <p>Loading...</p>
          ) : error ? (
            <p>Error: {error}</p>
          ) : (
            <div>
              <div>
                <span className="user-info-description font-bold">
                  Total posts:{" "}
                </span>
                <span className="user-info-value">
                  {profileUser.totalPosts ?? 0}
                </span>
              </div>

              {posts.map((post, index) => {
                if (posts.length === index + 1) {
                  return (
                    <div
                      key={post.id}
                      className="post-wrapper p-2 text-gray-500 bg-[#191919] rounded-md border-transparent my-4"
                      ref={lastPostElementRef}
                    >
                      <Post post={post} />
                    </div>
                  );
                } else {
                  return (
                    <div
                      key={post.id}
                      className="post-wrapper p-2 text-gray-500  bg-[#191919] rounded-md border-transparent my-4"
                    >
                      <Post post={post} />
                    </div>
                  );
                }
              })}
            </div>
          )}
          {loading && posts.length !== 0 && <p>Loading more posts...</p>}
        </div>
      </div>
    </div>
  );
};

export default Profile;
