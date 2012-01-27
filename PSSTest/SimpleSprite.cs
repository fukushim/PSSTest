
using System;
using System.Collections.Generic;
using System.Diagnostics;

using Sce.Pss.Core;
using Sce.Pss.Core.Graphics;
using Sce.Pss.Core.Imaging;


namespace Sce.Pss.Utility
{

	/// <summary>
	/// スプライト
	/// </summary>
	public class SimpleSprite
	{
		protected GraphicsContext graphics;
		
		// 頂点座標。
		float[] vertices=new float[12];
		
		
		// テクスチャ座標。
		float[] texcoords = {
			0.0f, 0.0f,	// 左上
			0.0f, 1.0f,	// 左下
			1.0f, 0.0f,	// 右上
			1.0f, 1.0f,	// 右下
		};


		// 頂点色。
		float[] colors = {
			1.0f,	1.0f,	1.0f,	1.0f,	// 左上
			1.0f,	1.0f,	1.0f,	1.0f,	// 左下
			1.0f,	1.0f,	1.0f,	1.0f,	// 右上
			1.0f,	1.0f,	1.0f,	1.0f,	// 右下
		};
		
		// インデックス。
		const int indexSize = 4;
		ushort[] indices;
		
		// 頂点バッファ。
		VertexBuffer vertexBuffer;
		
		
		protected Texture2D texture;

		
		// プロパティではPosition.Xという記述ができないため、publicの変数にしています。
		/// <summary>
		/// スプライトの表示位置。
		/// </summary>
		public Vector3 Position ;
		
		/// <summary>
		/// スプライトの中心座標を指定。0.0f～1.0fの範囲で指定してください。
		/// スプライトの中心を指定する場合 X=0.5f, Y=0.5fとなります。
		/// </summary>
		public Vector2 Center;
		

		float width,height;
		/// <summary>
		/// スプライトの幅。
		/// </summary>
		public float Width 
		{
			get { return width;}
		}
		/// <summary>
		/// スプライトの高さ。
		/// </summary>
		public float Height 
		{
			get { return height;}
		}

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		public SimpleSprite(GraphicsContext graphics, Texture2D texture)
		{
			if (texture == null)
			{
				throw new Exception("ERROR: texture is null.");
			}

			this.graphics = graphics;
			this.texture = texture;
			this.width = texture.Width;
			this.height = texture.Height;

			indices = new ushort[indexSize];
			indices[0] = 0;
			indices[1] = 1;
			indices[2] = 2;
			indices[3] = 3;
			//indices[4] = 2;
			//indices[5] = 3;
			
			//												頂点座標,                テクスチャ座標,     頂点色
			vertexBuffer = new VertexBuffer(4, indexSize, VertexFormat.Float3, VertexFormat.Float2, VertexFormat.Float4);
			
		}


		// 頂点色の設定。
		public void SetColor(Vector4 color)
		{
			for (int i = 0; i < colors.Length; i+=4)
			{
				colors[i] = color.R;
				colors[i+1] = color.G;
				colors[i+2] = color.B;
				colors[i+3] = color.A;
			}
		}
		
		
		/// <summary>テクスチャ座標を指定します。ピクセル単位で指定してください。
		/// </summary>
		public void SetTextureCoord(float x0, float y0, float x1, float y1)
		{
			texcoords[0] = x0 / texture.Width;	// 左上u
			texcoords[1] = y0 / texture.Height; // 左上v
			
			texcoords[2] = x0 / texture.Width;	// 左下u
			texcoords[3] = y1 / texture.Height;	// 左下v
			
			texcoords[4] = x1 / texture.Width;	// 右上u
			texcoords[5] = y0 / texture.Height;	// 右上v
			
			texcoords[6] = x1 / texture.Width;	// 右下u
			texcoords[7] = y1 / texture.Height;	// 右下v
		
		}
		
		/// <summary>テクスチャ座標を指定します。ピクセル単位で指定してください。
		/// </summary>
		public void SetTextureCoord(Vector2 topLeft, Vector2 bottomRight)
		{
			SetTextureCoord(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);
		}
		

		/// <summary>
		/// スプライトの描画。
		/// このメソッドを呼び出す前にシェーダプログラム Sprite.cgx をセットしておいてください。
		/// </summary>
		public void Render()
		{
			vertices[0]=Position.X + (-Width * Center.X);
			vertices[1]=Position.Y+(-Height * Center.Y);
			vertices[2]=Position.Z;
			vertices[3]=Position.X + (-Width * Center.X);
			vertices[4]=Position.Y+( Height * (1.0f-Center.Y));
			vertices[5]=Position.Z;
			vertices[6]=Position.X +  Width * (1.0f-Center.X);
			vertices[7]=Position.Y+(-Height * Center.Y);
			vertices[8]=Position.Z;
			vertices[9]=Position.X +  Width * (1.0f-Center.X);
			vertices[10]=Position.Y+( Height * (1.0f-Center.Y));
			vertices[11]=Position.Z;

			vertexBuffer.SetVertices(0, vertices);
			vertexBuffer.SetVertices(1, texcoords);
			vertexBuffer.SetVertices(2, colors);
			
			vertexBuffer.SetIndices(indices);
			graphics.SetVertexBuffer(0, vertexBuffer);
			graphics.SetTexture(0, texture);


			graphics.DrawArrays(DrawMode.TriangleStrip, 0, indexSize);
		}
	}
}


