import http from "k6/http";
import { sleep, check, group } from "k6";
import { FormData } from "https://jslib.k6.io/formdata/0.0.2/index.js";
const token =
  "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6IjExMDMwNjAxOUBuY2N1LmVkdS50dyIsIlJvbGUiOiJOQ0NVU3R1ZGVudCIsIlVzZXJJZCI6IjY0Y2QxYTE0NjEwMjgzNjYzNjRlM2YxMiIsImV4cCI6MTcyNjAxNzQwMCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzI0MyIsImF1ZCI6ImZyb250LWVuZC11cmwifQ.Y9dgSV07T4ow_m9TzKHJPswgBLTo8DOAEP4CsRDh2os";
const url = "https://localhost:7243/nccupass/NormalTask/create";
const url2 =
  "https://localhost:7243/nccupass/Task/hashtag-search/%23stress/1/0";

const img = open("hi.png");

export const options = {
  stages: [
    { duration: '1m', target: 10 }, // Maintain a steady load of 10 VUs for 1 minute.
    { duration: '1m', target: 50 }, // Spike: Increase to 50 VUs for 1 minute.
    { duration: '1m', target: 10 }, // Back to the baseline of 10 VUs for 1 minute.
    { duration: '1m', target: 100 }, // Spike: Increase to 100 VUs for 1 minute.
    { duration: '1m', target: 10 }, // Back to the baseline of 10 VUs for 1 minute.
    { duration: '1m', target: 200 }, // Spike: Increase to 200 VUs for 1 minute.
  ],
};

export default function () {
  const fd = new FormData();

  fd.append("Title", "test");
  fd.append("Description", "stress");
  fd.append("TaskImages", http.file(img, "hi.png"));
  fd.append("TaskHashTags", "#stress");
  fd.append("StartTime", "2023-08-26T06:35:00+08:00");
  fd.append("EndTime", "2023-08-31T06:35:00+08:00");
  fd.append("NeededPeopleNum", "1");
  fd.append("Pay", "20");
  fd.append(
    "StartLocation.Name",
    "小琉球套裝民宿｜你我的相遇－寵物友善3D創意民宿"
  );
  fd.append("StartLocation.Address", "台灣屏東縣復興路");
  fd.append("StartLocation.Lat", "22.342104");
  fd.append("StartLocation.Lng", "120.3654653");
  fd.append("StartLocation.GooglePlaceId", "ChIJAZ4t2fTvcTQRHrciYv8tFr4");
  fd.append(
    "EndLocation.Name",
    "小琉球套裝民宿｜你我的相遇－寵物友善3D創意民宿"
  );
  fd.append("EndLocation.Address", "台灣屏東縣復興路");
  fd.append("EndLocation.Lat", "22.342104");
  fd.append("EndLocation.Lng", "120.3654653");
  fd.append("EndLocation.GooglePlaceId", "ChIJAZ4t2fTvcTQRHrciYv8tFr4");
  fd.append("PaymentMethods", "None");
  const params = {
    headers: {
      accept: "text/plain",
      Authorization: token,
      "Content-Type": `multipart/form-data; boundary=${fd.boundary}`,
    },
  };
  const params2 = {
    headers: {
      accept: "text/plain",
      Authorization: token,
    },
  };

  group("create and search and get detail", function () {
    const res = http.post(url, fd.body(), params);
    check(res, { "status of create was 200": (r) => r.status == 200 });
    const res2 = http.get(url2, params2);
    check(res2, { "status of search was 200": (r) => r.status == 200 });
    const taskId = res2.body.substring(92, 116);
    const url3 = "https://localhost:7243/nccupass/Task/" + taskId;
    const res3 = http.get(url3, params2);
    check(res3, { "status of get detail was 200": (r) => r.status == 200 });
  });
  sleep(1);
}