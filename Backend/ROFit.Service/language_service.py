from fastapi import FastAPI, File, UploadFile
from transformers import pipeline
from ultralytics import YOLO
from PIL import Image
from io import BytesIO
import torch

app = FastAPI()

@app.post("/predict-food")
async def predict_food(file: UploadFile = File(...)):
    image_bytes = await file.read()
    detected_food = detect_food(image_bytes)
    return {"predictions": detected_food}


model = YOLO("yolov8n.pt")

def detect_food(image_bytes):
    image = Image.open(BytesIO(image_bytes)).convert("RGB")
    results = model(image)

    detected_food = []
    for result in results:
        for box in result.boxes:
            cls_id = int(box.cls[0])           
            label = model.names[cls_id]       
            detected_food.append({
                "label": label,
                "confidence": float(box.conf[0]),
                "bbox": [round(coord, 2) for coord in box.xyxy[0].tolist()]
            })
    return detected_food
